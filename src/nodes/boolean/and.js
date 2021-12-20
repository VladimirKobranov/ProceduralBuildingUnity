const AbstractNode = require('../abstract')

class BooleanAnd extends AbstractNode {

  static metadata() {
    return {
      name: 'A && B',
      code: 'boolean/and',
      type: 'modifier',
      deleteable: true,
      addable: true,
      inputs: {
        valA: {
          code: 'valA',
          name: 'A',
          type: 'basic/boolean'
        },
        valB: {
          code: 'valB',
          name: 'B',
          type: 'basic/boolean'
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
    this.log('execute', inputs)
    this.setOutput('result', !!inputs.valA && !!inputs.valB)
  }
}

module.exports = BooleanAnd
