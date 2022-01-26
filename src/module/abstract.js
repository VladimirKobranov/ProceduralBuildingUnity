const EventEmitter = require('events')

class AbstractModule extends EventEmitter {

  static metadata() {
    return {
      code: 'abstract',
      name: 'Abstract Module'
    }
  }

  metadata() {
    return this.constructor.metadata()
  }

  constructor(vm) {
    super()
    this._vm = vm
  }

  vm() { return this._vm }

  async start () {}
  async stop () {}

  libraryUpdate() {}
}

module.exports = AbstractModule
