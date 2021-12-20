const AbstractNode = require('../abstract')

class StructFromObject extends AbstractNode {

  static metadata() {
    return {
      name: 'From Object',
      code: 'struct/fromobject',
      type: 'modifier',
      deleteable: true,
      addable: false,
      inputs: {
        // updates automatically by struct
        object: {
          name: 'Object',
          code: 'object',
          type: 'bluep/object'
        }
      },
      outputs: {
        struct: {
          name: 'Structure',
          code: 'struct',
          type: 'bluep/struct'
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
    this.setOutput('struct', inputs.object)
  }
}

module.exports = StructFromObject
