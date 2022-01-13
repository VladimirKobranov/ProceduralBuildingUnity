const AbstractNode = require('../abstract')

class Return extends AbstractNode {

  static metadata() {
    return {
      name: 'Return',
      code: 'graph/return',
      type: 'execute',
      deleteable: true,
      addable: true,
      outputs: {},
      inputs: {
        call: {
          code: 'call',
          name: 'Return',
          type: 'basic/execute'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    Object.keys(this._node.inputs).forEach(key => {
      const slot = this._node.inputs[key]
      if (slot.type === 'basic/execute') return
      this.setGraphOutput(key, inputs[key])
    })
    return 'return'
  }
}

module.exports = Return
