const AbstractNode = require('../abstract')

class EnumEqual extends AbstractNode {

  static metadata() {
    return {
      name: 'Equal',
      code: 'enum/equal',
      type: 'modifier',
      deleteable: true,
      addable: false,
      inputs: {
        vala: {
          code: 'vala',
          name: 'A',
          type: 'bluep/enum'
        },
        valb: {
          code: 'valb',
          name: 'B',
          type: 'bluep/enum'
        }
      },
      outputs: {
        eq: {
          code: 'eq',
          name: 'Equal',
          type: 'basic/boolean'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    const ret = inputs.vala === inputs.valb
    this.setOutput('eq', ret)
  }
}

module.exports = EnumEqual
