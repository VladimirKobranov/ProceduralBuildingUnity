const BaseTypes = require('./types')
const BaseNodes = require('./nodes/')
const Graph = require('./graph')

const CronModule = require('./module/cron')
const ActorModule = require('./module/actor')

/**
 * @bluepjs Virtual Machine class
 */

class Vm {
  /**
   *  Creates new Virtual Machine
   *  @constructor
   *  @param {boolean} [debug=false] - enables debug info
   */
  constructor(debug = false) {
    this._libraries = null
    this._types = { ...BaseTypes }
    this._nodes =  {}
    this._modules =  {}
    this._debug = debug
    this._console = {
      log: console.log,
      error: console.error,
      debug: (...args) => {
        if (this._debug) {
          console.log('DEBUG#', ...args)
        }
      }
    }
    this._run = false
    // loading base nodes
    Object.values(BaseNodes).forEach(Node => this.registerNode(Node))
    // adding base modules
    this.addModule(CronModule)
    this.addModule(ActorModule)
  }

  /**
   *  Get VM Module shortcut
   *  @param {string} code - module code
   *  @returns {AbstractModule|undefined} module object or undefined
   */
  M(code) {
    return this._modules[code]
  }

  /**
   *  Get VM actor shortcut
   *  @param {string} id - actor id
   *  @returns {AbstractActor|undefined} actor object or undefined
   */
  A(id) {
    return this.actor(id)
  }
  // /SHORTCUTS

  /**
   *  Get VM actors metadata
   *  @returns {Object} actors metadata map indexed by actor id
   */
  actors() {
    if (!this.M('actor'))
      return {}
    return Object.values(this.M('actor').actors())
      .filter(x => !!x)
      .map(a => a.metadata())
      .reduce((acc,a) => { acc[a.id] = a; return acc }, {})
  }

  /**
   *  Get VM actor
   *  @param {string} id - actor id
   *  @returns {AbstractActor|undefined} actor object or undefined
   */
  actor(id) {
    if (!this.M('actor'))
      return null
    return this.M('actor').actor(id)
  }

  // RUN
  /**
   *  VM is running getter
   *  @returns {boolean} VM running state
   */
  running () { return this._run }

  /**
   *  VM start method
   */
  start () {
    this._run = true
    Object.values(this._modules).forEach(m => m.start())
  }

  /**
   *  VM stop method
   */
  stop () {
    this._run = false
    Object.values(this._modules).forEach(m => m.stop())
  }
  // /RUN

  /**
   *  Add new module to VM
   *  @param {AbstractModule} Module - module class. should implement Abstract Module interface
   */
  addModule(Module) {
    const info = Module.metadata()
    this._modules[info.code] = new Module(this)
  }

  /**
   *  Getter/setter of VM console
   *  @param {Object} [next] - console object. Should has ``log``, ``error`` and ``debug`` methods.
   *  @returns {Object} VM console object
   */
  console(next) {
    if (next && typeof next.log === 'function' && typeof next.error === 'function' && typeof next.debug === 'function')
      this._console = next
    return this._console
  }

  /**
   *  Register Node class
   *  @param {AbstractNode} Class - node class. should implement Abstract Node interface
   */
  registerNode(Class) {
    const meta = Class.metadata()
    this._nodes[meta.code] = Class
  }

  /**
   *  Node class by node code
   *  @param {string} code - node class code.
   *  @returns {Class} node class
   */
  nodeClass(code) {
    const ret = this._nodes[code]
    if (ret)
      return ret
    const cls = Object.keys(this._nodes).find(key => code.startsWith(key))
    return this._nodes[cls]
  }

  /**
   *  Information about VM for IDE
   *  @returns {Object} information object with types, nodes, modules, actors and libraries
   */
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

  /**
   *  Base @bluepjs types (for IDE)
   *  @returns {Object} base types map indexed by type code
   */
  types() {
    return this._types
  }

  /**
   *  VM nodes metadata (for IDE)
   *  @returns {Object[]} array of registered nodes classes
   */
  nodes() {
    return Object.values(this._nodes)
      .map(Node => ({ ...Node.metadata(), of: 'node' }))
  }

  /**
   *  VM modules metadata (for IDE)
   *  @returns {Object} modules metadata map indexed by module code
   */
  modules() {
    return Object.values(this._modules)
      .map(m => {
        const info = m.constructor.metadata() 
        const ide = m.constructor.ide()
        if (!Object.keys(ide || {}).length)
          return info
        info.ide = {}
        // when VM runing in browser - we don't need to convert functions
        // when runing in nodejs (backend) - ide methods should be stringified
        if (ide.nodes) {
          if (typeof window !== 'undefined')
            info.ide.nodes = ide.nodes
          else
            info.ide.nodes = ide.nodes.toString()
        }
        return info
      })
      .reduce((acc,m) => { acc[m.code] = m; return acc }, {})
  }

  /**
   *  Getter/setter of VM libraries. Use as setter with care, because as setter - method doesn't informs modules about update
   *  @param {Object} [next] - new libraries object.
   *  @returns {Object} VM libraries object
   */
  libraries(next) {
    if (next) this._libraries = next
    return this._libraries
  }

  /**
   *  Setter of VM libraries. Use as main libraries setter, because method do modules update also
   *  @param {Object} [next] - new libraries object.
   */
  updateLibraries(libs) {
    this.libraries(libs)
    Object.keys(this._modules).forEach(code => {
      this._modules[code].libraryUpdate()
    })
  }

  /**
   *  Run Blueprint function from library by info object. Uses ``runLibraryFunction`` method.
   *  @async
   *  @param {Object} info - information to run:
   *  @param {string} info.library - library code where function located
   *  @param {string} info.code - function code
   *  @param {Object} info.inputs - function inputs
   *  @returns {any} function result
   */
  async runFunction(info) {
    if (!this._run || !this._libraries || !this._libraries[info.library] || !this._libraries[info.library].functions) return
    const fn = Object.values(this._libraries[info.library].functions).find(f => f.code === info.code)
    if (!fn) return
    const res = await this.runLibraryFunction(fn.library, fn.code, info.inputs)
    return res
  }

  /**
   *  Run Blueprint function from library by params.
   *  @async
   *  @param {string} lib - library code where function located
   *  @param {string} fn - function code
   *  @param {Object} inputs - function inputs
   *  @returns {any} function result
   */
  async runLibraryFunction(lib, fn, inputs) {
    if (!this._run || !this._libraries || !this._libraries[lib] || !this._libraries[lib].functions || !this._libraries[lib].functions[fn]) return
    this.console().debug('Vm::runfn', lib, fn, inputs)
    const graph = this._libraries[lib].functions[fn]
    const bp = new Graph(this)
    bp.load(graph)
    await bp.execute(inputs)
    this.console().debug('Vm::executed', bp.name(), bp.getResult())
    return bp.getResult()
  }

}

module.exports = Vm
