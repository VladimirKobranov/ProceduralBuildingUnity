const AbstractNode = require('../abstract')

class ClassCastTo extends AbstractNode {

  static metadata() {
    return {
      name: 'Get Class Variable',
      code: 'class/castto',
      type: 'modifier',
      deleteable: true,
      addable: false,
      inputs: {
        object: {
          code: 'object',
          name: 'Object',
          type: 'bluep/class'
        }
      },
      outputs: {
        object: {
          code: 'object',
          name: 'Object',
          type: 'bluep/class'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', this._node.code)
    this.setOutput('object', inputs.object)
  }
}

module.exports = ClassCastTo
