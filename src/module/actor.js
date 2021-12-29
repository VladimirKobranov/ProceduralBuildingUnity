const dayjs = require('dayjs')
const AbstractModule = require('./abstract')

const ActorState = require('./actor/node.state')
const ActorMethod = require('./actor/node.method')

class ActorModule extends AbstractModule {

  static metadata() {
    return {
      code: 'actor',
      name: 'Actor',
      events: {
        start: {
          code: 'start',
          name: 'VM Start',
          outputs: {
            now: {
              code: 'now',
              name: 'Now',
              type: 'basic/datetime'
            }
          }
        }
      }
    }
  }

  static ide() {
    return {
      nodes: (libraries, currentLibrary, actors, nodes) => {
        const ret = []
        // console.log('nodes', {libraries, currentLibrary, actors, nodes})
        const actorState = nodes.find(node => node.code === 'actor/state')
        const actorMethod = nodes.find(node => node.code === 'actor/method')
        Object.values(actors || {}).forEach(actor => {
          const nodeState = JSON.parse(JSON.stringify(actorState))
          nodeState.addable = true
          nodeState.code += `/${actor.id}/state`
          nodeState.name = actor.name + ' state'
          nodeState.data = {
            actor: actor.id
          }
          Object.keys(actor.state).forEach(field => {
            nodeState.outputs[field] = JSON.parse(JSON.stringify(actor.state[field]))
          })
          ret.push(nodeState)

          Object.values(actor.methods || {}).forEach(method => {
            const nodeMethod = JSON.parse(JSON.stringify(actorMethod))
            nodeMethod.addable = true
            nodeMethod.code += `/${actor.id}/method/${method.code}`
            nodeMethod.name = actor.name + ' :: ' + method.name + '()'
            nodeMethod.data = {
              actor: actor.id,
              method: method.code
            }
            Object.keys(method.inputs || {}).forEach(field => {
              nodeMethod.inputs[field] = JSON.parse(JSON.stringify(method.inputs[field]))
            })
            Object.keys(method.outputs || {}).forEach(field => {
              nodeMethod.outputs[field] = JSON.parse(JSON.stringify(method.outputs[field]))
            })
            ret.push(nodeMethod)
          })
        })
        return ret
      // },
      // types: (libraries, currentLibrary, actors, types) => {
        // console.log('types', {libraries, currentLibrary, actors, types})
      }
    }
  }

  start () {
    const libs = this._vm.libraries()
    const functions = libs && libs.default && libs.default.functions ? libs.default.functions : {}
    const starts = Object.values(functions).filter(f => f.event && f.event.module === 'actor' && f.event.code === 'start')
    const now = dayjs()
    starts.forEach(fn => {
      this._vm.runLibraryFunction('default', fn.code, {now})
    })
  }

  constructor(vm) {
    super(vm)

    this._actors = {}
    this._actorsEvents = {}

    this._vm.registerNode(ActorState)
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
      actor.on(event.event, inputs => {
        if (!this._vm.running())
          return
        if (this._debug)
          this.console().log('actor event!', id, event.event, inputs)
        if (this._actorsEvents[id] && Array.isArray(this._actorsEvents[id][event.event])) {
          this._actorsEvents[id][event.event].forEach(fn => {
            if (this._debug)
              this.console().log('Vm::actorEvent', id, info, event, inputs)
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
