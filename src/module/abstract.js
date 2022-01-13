class AbstractModule {

  static metadata() {
    return {
      code: 'abstract',
      name: 'Abstract Module'
    }
  }

  constructor(vm) {
    this._vm = vm
  }

  async start () {}
  async stop () {}

  libraryUpdate() {}
}

module.exports = AbstractModule
