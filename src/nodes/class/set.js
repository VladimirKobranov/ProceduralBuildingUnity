const AbstractNode = require('../abstract')

class ClassVariableSet extends AbstractNode {

  static metadata() {
    return {
      name: 'ClassVariableSet',
      code: 'class/set',
      type: 'execute',
      deleteable: true,
      addable: false,
      inputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        },
        object: {
          code: 'object',
          name: 'Object',
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
    this.debug('execute', this._node.code, inputs)
    if (!inputs.object) return 'return'
    inputs.object[this._node.data.code] = inputs[this._node.data.code]
    return 'return'
  }
}

module.exports = ClassVariableSet
