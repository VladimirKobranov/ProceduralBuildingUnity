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
    this._subsIndex = []

    this._vm.registerNode(ActorState)
    this._vm.registerNode(ActorGet)
    this._vm.registerNode(ActorMethod)
  }

  libraryUpdate () {
    this.updateActorsEventsIndex(true)
  }

  addActor(actor) {
    actor.vm(this._vm)
    this._actors[actor.id()] = actor
    this.updateActorsEventsIndex()
  }

  removeActor(actor) {
    actor.vm(null)
    const id = actor.id()
    if (this._actors[id]) {
      this._actors[id].removeAllListeners()
      this._subsIndex = this._subsIndex.filter(x => !x.startsWith(`${id}/`))
    }
    this._actors[id] = null
    this.updateActorsEventsIndex()
  }

  actors() {
    return this._actors
  }

  actor(id) {
    return this._actors[id]
  }

  updateActorsEventsIndex(libUpdate = false) {
    if (libUpdate) {
      this._actorsEvents = {}
      const libs = this._vm.libraries()
      if (!libs || !libs.default || !libs.default.functions)
        return
      Object.values(libs.default.functions || {}).forEach(fn => {
        if (!fn.event || !fn.event.actor)
          return
        if (!this._actorsEvents[fn.event.actor])
          this._actorsEvents[fn.event.actor] = {}
        if (!this._actorsEvents[fn.event.actor][fn.event.code])
          this._actorsEvents[fn.event.actor][fn.event.code] = []
        this._actorsEvents[fn.event.actor][fn.event.code].push(fn.code)
      })
    }
    Object.keys(this._actorsEvents || {}).forEach(aid => {
      if (!this._actors[aid]) return
      Object.keys(this._actorsEvents[aid] || {}).forEach(eventCode => {
        if (this._subsIndex.includes(`${aid}/${eventCode}`)) return
        this._subsIndex.push(`${aid}/${eventCode}`)
        this._actors[aid].on(eventCode, inputs => {
          if (!this._vm.running())
            return
          this._vm.console().debug('actor event!', { aid, eventCode, inputs })
          if (this._actorsEvents[aid] && Array.isArray(this._actorsEvents[aid][eventCode])) {
            this._actorsEvents[aid][eventCode].forEach(fn => {
              this._vm.console().debug('Vm::actorEvent', aid, eventCode, fn, inputs)
              this._vm.runLibraryFunction('default', fn, inputs)
            })
          }
        })
      })
    })
  }

}

module.exports = ActorModule
