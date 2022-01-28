const dayjs = require('dayjs')
const AbstractModule = require('./abstract')

const ActorGet = require('./actor/node.get')
const ActorState = require('./actor/node.state')
const ActorMethod = require('./actor/node.method')

class ActorModule extends AbstractModule {

  static metadata() {
    return {
      code: 'actor',
      name: 'Actor',
      classes: {}
    }
  }

  constructor(vm) {
    super(vm)

    this._actors = {}
    this._actorsEvents = {}

    this._vm.registerNode(ActorState)
    this._vm.registerNode(ActorGet)
    this._vm.registerNode(ActorMethod)
  }

  libraryUpdate () {
    this.updateActorsEventsIndex()
  }

  addActor(actor) {
    actor.vm(this._vm)
    const info = actor.constructor.metadata()
    const id = actor.id()
    this._actors[id] = actor
    this.updateActorsEventsIndex()
    Object.values(info.events || {}).forEach(event => {
      actor.on(event.code, inputs => {
        if (!this._vm.running())
          return
        this._vm.console().debug('actor event!', id, event.code, inputs)
        if (this._actorsEvents[id] && Array.isArray(this._actorsEvents[id][event.code])) {
          this._actorsEvents[id][event.code].forEach(fn => {
            this._vm.console().debug('Vm::actorEvent', id, info, event, inputs)
            this._vm.runLibraryFunction('default', fn, inputs)
          })
        }
      })
    })
  }

  removeActor(actor) {
    actor.vm(null)
    const info = actor.constructor.metadata()
    const id = actor.id()
    if (this._actors[id])
      this._actors[id].removeAllListeners()
    /*
    Object.values(info.events || {}).forEach(event => {
      this._actors[id].removeAllListeners(event.event)
    })
    */
    this._actors[id] = null
    this.updateActorsEventsIndex()
  }

  actors() {
    return this._actors
  }

  actor(id) {
    return this._actors[id]
  }

  updateActorsEventsIndex() {
    this._actorsEvents = {}
    const libs = this._vm.libraries()
    if (!libs || !libs.default || !libs.default.functions)
      return
    Object.values(libs.default.functions || {}).forEach(fn => {
      if (!fn.event || !fn.event.actor || !this._actors[fn.event.actor])
        return
      if (!this._actorsEvents[fn.event.actor])
        this._actorsEvents[fn.event.actor] = {}
      if (!this._actorsEvents[fn.event.actor][fn.event.code])
        this._actorsEvents[fn.event.actor][fn.event.code] = []
      this._actorsEvents[fn.event.actor][fn.event.code].push(fn.code)
    })
  }

}

module.exports = ActorModule
