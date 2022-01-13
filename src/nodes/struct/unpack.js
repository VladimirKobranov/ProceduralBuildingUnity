const AbstractNode = require('../abstract')

class StructUnpack extends AbstractNode {

  static metadata() {
    return {
      name: 'Unpack',
      code: 'struct/unpack',
      type: 'modifier',
      deleteable: true,
      addable: false,
      inputs: {
        struct: {
          name: 'Structure',
          code: 'struct',
          type: 'bluep/struct'
        }
      },
      outputs: {
        // builds automatically by this._node.data.schema
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    const obj = inputs.struct || {}
    Object.keys(this._node.data.schema).forEach(code => {
      this.setOutput(code, obj[code])
    })
  }
}

module.exports = StructUnpack
