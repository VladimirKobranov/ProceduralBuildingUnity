const AbstractNode = require('../abstract')

class VariableSet extends AbstractNode {

  static metadata() {
    return {
      name: 'VariableSet',
      code: 'variable/set',
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
    this.log('execute', this._node.code, inputs)
    this._context.setOutput(this._node.data.context, this._node.data.code, inputs[this._node.data.code])
    return 'return'
  }
}

module.exports = VariableSet
