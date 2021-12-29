class AbstractModule {

  static metadata() {
    return {
      code: 'abstract',
      name: 'Abstract Module'
    }
  }

  /* define to work on ide side */
  static ide() {
    return null
    /*
    return {
      nodes: (libraries, currentLibrary, actors, nodes) => {},
      types: (libraries, currentLibrary, actors, types) => {}
    }
    */
  }

  constructor(vm) {
    this._vm = vm
  }

  async start () {}
  async stop () {}

  libraryUpdate() {}
}

module.exports = AbstractModule
