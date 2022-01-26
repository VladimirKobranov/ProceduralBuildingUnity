const BaseTypes = require('./types')
const BaseNodes = require('./nodes/')
const Graph = require('./graph')

const CoreModule = require('./module/core')
const ActorModule = require('./module/actor')
const CronModule = require('./module/cron')

const jclone = obj => JSON.parse(JSON.stringify(obj))

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
    this.addModule(CoreModule)
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
      .map(a => {
        const ret = a.metadata()
        ret._metadata = a.constructor.metadata()
        return ret
      })
      .filter(a => !a.internal)
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
  async start () {
    this._run = true
    await Promise.all(Object.values(this._modules).map(m => m.start()))
  }

  /**
   *  VM stop method
   */
  async stop () {
    this._run = false
    await Promise.all(Object.values(this._modules).map(m => m.stop()))
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
        const info = m.metadata() 
        info._metafata = m.constructor.metadata()
        // const ide = m.constructor.ide()
        // if (!Object.keys(ide || {}).length)
        return info
        /*
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
        */
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

  /**
   *  Run Blueprint class constructor from library by params.
   *  @async
   *  @param {object} self - object to apply constructor
   *  @param {string} lib - library code where function located
   *  @param {string} fn - constructor code
   *  @param {Object} inputs - function inputs
   *  @returns {any} function result
   */
  async runLibraryConstructor(self, lib, cls, fn, inputs) {
    if (!this._run || !this._libraries || !this._libraries[lib] || !this._libraries[lib].classes) return
    this.console().debug('Vm::class::constructor', lib, cls, fn, inputs)
    // cloning, because we modifying context inputs with "this"
    const graph = JSON.parse(JSON.stringify(this._libraries[lib].classes[cls].methods[fn]))
    const bp = new Graph(this)
    bp.self(self)
    bp.load(graph)
    await bp.execute(inputs)
    this.console().debug('Vm::constructor::executed', bp.name(), bp.getResult())
    return bp.getResult()
  }

  /**
   *  Run Blueprint class method from library by params.
   *  @async
   *  @param {object} self - object to apply constructor
   *  @param {string} lib - library code where function located
   *  @param {string} fn - function code
   *  @param {Object} inputs - function inputs
   *  @returns {any} function result
   */
  async runLibraryMethod(self, lib, cls, fn, inputs) {
    if (!this._run || !this._libraries || !this._libraries[lib] || !this._libraries[lib].classes || !this._libraries[lib].classes[cls] || !this._libraries[lib].classes[cls].methods[fn]) return
    this.console().debug('Vm::class::method', lib, cls, fn, inputs)
    // cloning, because we modifying context inputs with "this"
    const graph = JSON.parse(JSON.stringify(this._libraries[lib].classes[cls].methods[fn]))
    const bp = new Graph(this)
    bp.self(self)
    bp.load(graph)
    await bp.execute(inputs)
    this.console().debug('Vm::class::method::executed', bp.name(), bp.getResult())
    return bp.getResult()
  }


  /**
   *  Simple run module event function (without configurstion) from library by info object. Uses ``runLibraryFunction`` method. Can be used (when event configuration not exists or doesn't matter) as simple way to trigger event in modules.
   *  @async
   *  @param {Object} info - information to run:
   *  @param {string} [info.library = 'default'] - library code where event functions located
   *  @param {string} info.module - module code
   *  @param {string} info.event - module event code
   *  @param {Object} inputs - function inputs (event outputs)
   */
  async runModuleEvent(info, inputs) {
    const lib = info.library || 'default'
    if (!this._run || !this._libraries || !this._libraries[lib]) return
    const fns = Object.values(this._libraries[lib].functions).filter(f => f.event && f.event.module === info.module && f.event.code === info.event)
    fns.forEach(fn => {
      this.runLibraryFunction(lib, fn.code, inputs)
    })
  }

  ////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////

  //                   UTILS

  ////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////

  classLibrary (code) {
    let lib = null
    Object.keys(this._libraries).forEach(l => {
      if (this._libraries[l].classes && Object.keys(this._libraries[l].classes).includes(code)) lib = l
    })
    return lib
  }

  /**
   *  Class parents classes
   *  @param {string} classCode - class code
   *  @param {string} library - class library
   *  @returns {object} class parents. direct and deep extended.
   */
   classParents (classCode) {
    const ret = { direct: [], back: [] }
    const library = this.classLibrary(classCode)
    if (!library) return ret
    const cls = this._libraries[library].classes[classCode]
    if (!cls || !Object.keys(cls.extends || {}).length) return ret
    const modules = this.modules()
    Object.values(cls.extends).forEach(ext => {
      const inf = { ...ext }
      if (ext.module && modules && modules[ext.module] && modules[ext.module].classes[ext.code]) {
        inf.src = modules[ext.module].classes[ext.code]
      }
      if (inf.library) {
        inf.src = this._libraries[inf.library].classes[inf.code]
        const parents = this.classParents(inf.code)
        ret.back = [...ret.back, ...parents.direct, ...parents.back]
      }
      ret.direct.push(inf)
    })
    ret.back = ret.back.filter((cls, i, self) => self.indexOf(cls) === i)
    ret.index = [...ret.direct, ...ret.back].map(c => {
      if (c.library) return `library/${c.library}/${c.code}`
      return `module/${c.module}/${c.code}`
    })
    return ret
  }

  /**
   *  Class combined
   *  @param {string} classCode - class code
   *  @param {string} library - class library
   *  @param {object} libraries - libraries object
   *  @param {object} modules - modules metadata
   *  @returns {object|null} full class combined object
   */
  /**/
  classCombined (clsCode) {
    const lib = this.classLibrary(clsCode)
    if (!lib) return null
    const cls = jclone(this._libraries[lib].classes[clsCode])
    const parents = this.classParents(clsCode, lib)
    const list = [...parents.direct, ...parents.back]
    cls.deep = {
      schema: {},
      methods: {},
      overrides: {},
      parents: []
    }
    list.forEach(parent => {
      cls.deep.parents.push(parent.src.code)
      // properties
      Object.keys(parent.src.schema || {}).forEach(fld => {
        // if (parent.src.schema[fld].access === 'private') return
        cls.deep.schema[fld] = jclone(parent.src.schema[fld])
        cls.deep.schema[fld].source = {
          library: parent.library,
          libraryName: this._libraries[parent.library].name,
          code: parent.src.code,
          name: parent.src.name
        }
      })
      Object.keys(parent.src.methods || {}).forEach(mcode => {
        // if (parent.src.methods[mcode].access === 'private') return
        // parent method is overriden in class
        const over = Object.values(cls.methods || {}).find(m => m.overrides === mcode)
        if (over) {
          cls.deep.overrides[mcode] = jclone(parent.src.methods[mcode])
          cls.deep.overrides[mcode].source = {
            library: parent.library,
            libraryName: this._libraries[parent.library].name,
            code: parent.src.code,
            name: parent.src.name
          }
          return
        }
        // parent method is overriden in other parent class
        if (Object.values(cls.deep.methods || {}).find(m => m.overrides === mcode)) return
        // or already was checked and moved to overrides
        if (Object.values(cls.deep.overrides || {}).find(m => m.overrides === mcode)) return
        cls.deep.methods[mcode] = jclone(parent.src.methods[mcode])
        cls.deep.methods[mcode].source = {
          library: parent.library,
          libraryName: this._libraries[parent.library].name,
          code: parent.src.code,
          name: parent.src.name
        }
        // parent method overrides other deep method
        const po = parent.src.methods[mcode].overrides
        const md = cls.deep.methods[po]
        if (po && md) {
          const over = jclone(cls.deep.methods[po])
          delete cls.deep.methods[po]
          cls.deep.overrides[po] = over
        }
      })
    })
    return cls
  }
  /**/
}

module.exports = Vm
