const AbstractNode = require('../../nodes/abstract')

class ActorState extends AbstractNode {

  static metadata() {
    return {
      name: 'Actor States',
      code: 'actor/state',
      type: 'execute',
      deleteable: true,
      addable: false,
      inputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        },
        actor: {
          code: 'actor',
          name: 'Actor',
          type: 'bluep/class'
        }
      },
      outputs: {
        return: {
          code: 'return',
          name: 'Return',
          type: 'basic/execute'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', this._node.data.actor)
    const actor = inputs.actor
    if (!actor) {
      return 'return'
    }
    const states = actor.state() || {}
    Object.keys(states).forEach(key => {
      this.setOutput(key, states[key])
    })
    this.setOutput('success', true)
    return 'return'
  }
}

module.exports = ActorState
