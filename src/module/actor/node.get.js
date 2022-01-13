const AbstractNode = require('../../nodes/abstract')

class ActorGet extends AbstractNode {

  static metadata() {
    return {
      name: 'Actor Get',
      code: 'actor/get',
      type: 'getter',
      deleteable: true,
      addable: false,
      inputs: {
      },
      outputs: {
        actor: {
          code: 'actor',
          name: 'Actor',
          type: 'bluep/class'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', this._node.data, inputs)
    const actor = this.vm().M('actor').actor(this._node.data.actor)
    this.setOutput('actor', actor)
  }
}

module.exports = ActorGet
