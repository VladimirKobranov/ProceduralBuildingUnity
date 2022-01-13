const AbstractNode = require('../abstract')

class ClassVariableGet extends AbstractNode {

  static metadata() {
    return {
      name: 'Get Class Variable',
      code: 'class/get',
      type: 'getter',
      deleteable: true,
      addable: false,
      inputs: {
        object: {
          code: 'object',
          name: 'Object',
          type: 'bluep/class'
        }
      },
      outputs: {}
    }
  }

  async execute(inputs) {
    this.debug('execute', this._node.code)
    // console.log('get', this._node.code, inputs)
    if (!inputs.object) return
    this.setOutput(this._node.data.code, inputs.object[this._node.data.code])
  }
}

module.exports = ClassVariableGet
