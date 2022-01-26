const AbstractNode = require('../../nodes/abstract')

class ActorMethod extends AbstractNode {

  static metadata() {
    return {
      name: 'Actor Method',
      code: 'actor/method',
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
    this.debug('execute', this._node.data, inputs)
    const actor = inputs.actor
    if (!actor) {
      return 'return'
    }
    const fnInputs = {}
    Object.keys(inputs).forEach(incode => {
      if (incode === 'call' || incode === 'actor') return
      fnInputs[incode] = inputs[incode]
    })
    const result = await actor.method(this._node.data.method, fnInputs)
    const info = actor.metadata()
    const minfo = info.methods ? info.methods[this._node.data.method] : null
    if (minfo && result) {
      Object.keys(minfo.outputs || {}).forEach(key => {
        this.setOutput(key, result[key])
      })
    }
    return 'return'
  }
}

module.exports = ActorMethod
