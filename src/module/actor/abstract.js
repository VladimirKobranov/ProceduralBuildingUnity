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
      code: 'actor/abstract',
      name: 'Abstract Actor',
      internal: false, //to hide from ide
      state: {},
      methods: {},
      events: {}
    }
  }

  // should update metadata info with actor id and may append other specific info
  metadata () { return { id: this.id(), ...this.constructor.metadata() } }

  /**
    constructor

    @param id [string] - unique actor id.
  */
  constructor(id) {
    super()
    this._id = id || uuid()
    this._vm = null
    this._state = {}
  }

  // get/set id
  id(next) {
    if (typeof next !== 'undefined')
      this._id = next
    return this._id
  }

  // get/set vm
  vm(next) {
    if (typeof next !== 'undefined')
      this._vm = next
    return this._vm
  }

  // return full state object or only filed by code, if code if defined
  state(code) {
    if (code) return this._state[code]
    return this._state
  }

  // calls method 
  async method(method, inputs) {
    console.error('AbstractActor::method is not overriden in child actor!', method, inputs, this)
  }
}

module.exports = AbstractActor
