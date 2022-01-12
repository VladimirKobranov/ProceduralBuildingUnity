const AbstractNode = require('../abstract')

class StructPack extends AbstractNode {

  static metadata() {
    return {
      name: 'Pack',
      code: 'struct/pack',
      type: 'modifier',
      deleteable: true,
      addable: false,
      inputs: {
        // builds automatically by this._node.data.schema
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
    this.debug('execute', inputs)
    /*
    const ret = {}
    Object.keys(this._node.data.schema).forEach(code => {
      ret[code] = inputs[code]
    })
    console.log('ret', inputs, ret)
    this.setOutput('struct', ret)
    /**/
    // or
    this.setOutput('struct', inputs)
  }
}

module.exports = StructPack
