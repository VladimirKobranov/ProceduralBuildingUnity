const AbstractNode = require('../abstract')

class Call extends AbstractNode {

  static metadata() {
    return {
      name: 'Call',
      code: 'graph/call',
      type: 'execute',
      deleteable: false,
      addable: false,
      inputs: {},
      outputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    Object.keys(this._node.outputs).forEach(key => {
      const slot = this._node.outputs[key]
      if (slot.type === 'basic/execute') return
      this.setOutput(key, this._graph.getInput(key))
    })
    return 'call'
  }
}

module.exports = Call
