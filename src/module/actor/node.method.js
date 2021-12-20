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
    this.log('execute', this._node.data, inputs)
    const actor = this.vm().M('actor').actor(this._node.data.actor)
    if (!actor) {
      return 'return'
    }
    const result = await actor.method(this._node.data.method, inputs)
    const info = actor.constructor.metadata()
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
