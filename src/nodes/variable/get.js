const AbstractNode = require('../abstract')

class VariableGet extends AbstractNode {

  static metadata() {
    return {
      name: 'Get Variable',
      code: 'variable/get',
      type: 'getter',
      deleteable: true,
      addable: false,
      inputs: {},
      outputs: {}
    }
  }

  async execute(inputs) {
    this.log('execute', this._node.code)
    const val = this._context.getOutput(this._node.data.context, this._node.data.code)
    this.setOutput(this._node.data.code, val)
  }
}

module.exports = VariableGet
