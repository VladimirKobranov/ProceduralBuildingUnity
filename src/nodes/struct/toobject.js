const AbstractNode = require('../abstract')

class StructToObject extends AbstractNode {

  static metadata() {
    return {
      name: 'To Object',
      code: 'struct/toobject',
      type: 'modifier',
      deleteable: true,
      addable: false,
      inputs: {
        // updates automatically by struct
        struct: {
          name: 'Structure',
          code: 'struct',
          type: 'bluep/struct'
        }
      },
      outputs: {
        object: {
          name: 'Object',
          code: 'object',
          type: 'bluep/object'
        }
      }
    }
  }

  async execute(inputs) {
    this.log('execute', inputs)
    /*
    const ret = {}
    Object.keys(this._node.data.schema).forEach(code => {
      ret[code] = inputs[code]
    })
    console.log('ret', inputs, ret)
    this.setOutput('struct', ret)
    /**/
    // or
    this.setOutput('object', inputs.struct)
  }
}

module.exports = StructToObject
