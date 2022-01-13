const AbstractNode = require('../abstract')

class EnumToString extends AbstractNode {

  static metadata() {
    return {
      name: 'To String',
      code: 'enum/tostring',
      type: 'modifier',
      deleteable: true,
      addable: false,
      inputs: {
        option: {
          code: 'option',
          name: 'Value',
          type: 'bluep/enum'
        }
      },
      outputs: {
        string: {
          code: 'string',
          name: 'String',
          type: 'basic/string'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    const ret = this._node.data.values[inputs.option]
    this.setOutput('string', `${ret}`)
  }
}

module.exports = EnumToString
