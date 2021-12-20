const EventEmitter = require('events')
const uuid = require('uuid').v4

/**
  Actor interface, required to be implemented be developer
  when developing integration, if developed actor can't extended from AbstractActor

    1. should implement EventEmitter::on() subscription function
    2. should implement described functions

*/
class AbstractActor extends EventEmitter {

  static metadata() {
    return {
      code: 'abstract',
      name: 'Abstract Actor',
      state: {},
      methods: {},
      events: {}
    }
  }

  constructor(id) {
    super()
    this._id = id || uuid()
    this._vm = null
  }

  id(next) {
    if (typeof next !== 'undefined')
      this._id = next
    return this._id
  }

  vm(next) {
    if (typeof next !== 'undefined')
      this._vm = next
    return this._vm
  }

  // return full state object or only filed by code, if code if defined
  state(code) {}

  // calls method 
  async method(method, inputs) {}
}

module.exports = AbstractActor
