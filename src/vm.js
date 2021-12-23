const BaseTypes = require('./types')
const BaseNodes = require('./nodes/')
const Graph = require('./graph')

const CronModule = require('./module/cron')
const ActorModule = require('./module/actor')

class Vm {

  constructor(debug = false) {
    this._libraries = null
    this._types = { ...BaseTypes }
    this._nodes =  {}
    this._modules =  {}
    this._console = console
    this._debug = debug
    //loading base nodes
    Object.values(BaseNodes).forEach(Node => this.registerNode(Node))
    this.addModule(CronModule)
    this.addModule(ActorModule)
  }

  // SHORTCUTS
  M(code) {
    return this._modules[code]
  }

  addModule(Module) {
    const info = Module.metadata()
    this._modules[info.code] = new Module(this)
  }

  console(next) {
    if (next && typeof next.log === 'function' && typeof next.error === 'function')
      this._console = next
    return this._console
  }

  registerNode(Class) {
    const meta = Class.metadata()
    this._nodes[meta.code] = Class
  }

  nodeClass(code) {
    const ret = this._nodes[code]
    if (ret)
      return ret
    const cls = Object.keys(this._nodes).find(key => code.startsWith(key))
    return this._nodes[cls]
  }

  ideData() {
    const ret = {
      types: this.types(),
      nodes: this.nodes(),
      libraries: this.libraries(),
      modules: this.modules(),
      actors: this.actors()
    }
    return ret
  }

  types() {
    return this._types
  }

  nodes() {
    return Object.values(this._nodes)
      .map(Node => ({ ...Node.metadata(), of: 'node' }))
  }

  modules() {
    return Object.values(this._modules)
      .map(m => {
        const info = m.constructor.metadata() 
        const ide = m.constructor.ide()
        if (!Object.keys(ide || {}).length)
          return info
        info.ide = {}
        if (ide.nodes)
          info.ide.nodes = ide.nodes.toString()
        if (ide.types)
          info.ide.types = ide.types.toString()
        return info
      })
      .reduce((acc,m) => { acc[m.code] = m; return acc }, {})
  }

  actors() {
    if (!this.M('actor'))
      return {}
    return Object.values(this.M('actor').actors())
      .map(a => ({ id: a.id(), ...a.constructor.metadata() }))
      .reduce((acc,a) => { acc[a.id] = a; return acc }, {})
  }

  actor(id) {
    if (!this.M('actor'))
      return null
    return this.M('actor').actor(id)
  }

  libraries(next) {
    if (next) this._libraries = next
    return this._libraries
  }

  updateLibraries(libs) {
    this.libraries(libs)
    Object.keys(this._modules).forEach(code => {
      this._modules[code].libraryUpdate()
    })
  }

  async runFunction(info) {
    if (!this._libraries || !this._libraries[info.library] || !this._libraries[info.library].functions) return
    const fn = Object.values(this._libraries[info.library].functions).find(f => f.code === info.code)
    if (!fn) return
    const res = this.runLibraryFunction(fn.library, fn.code, info.inputs)
    return res
  }

  async runLibraryFunction(lib, fn, inputs) {
    if (!this._libraries || !this._libraries[lib] || !this._libraries[lib].functions || !this._libraries[lib].functions[fn]) return
    if (this._debug)
      this.console().log('Vm::runfn', lib, fn, inputs)
    const graph = this._libraries[lib].functions[fn]
    const bp = new Graph(this)
    bp.load(graph)
    await bp.execute(inputs)
    if (this._debug)
      this.console().log('Vm::executed', bp.name(), bp.getResult())
    return bp.getResult()
  }

}

module.exports = Vm
