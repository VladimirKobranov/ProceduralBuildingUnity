const AbstractNode = require('../abstract')

class StringEq extends AbstractNode {

  static metadata() {
    return {
      name: 'A === B',
      code: 'string/eq',
      type: 'modifier',
      deleteable: true,
      addable: true,
      inputs: {
        valA: {
          code: 'valA',
          name: 'A',
          type: 'basic/string'
        },
        valB: {
          code: 'valB',
          name: 'B',
          type: 'basic/string'
        }
      },
      outputs: {
        result: {
          code: 'result',
          name: 'Result',
          type: 'basic/boolean'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    this.setOutput('result', inputs.valA === inputs.valB)
  }
}

module.exports = StringEq
