const AbstractNode = require('../abstract')

class NumberIsGreater extends AbstractNode {

  static metadata() {
    return {
      name: 'A > B',
      code: 'number/gt',
      type: 'modifier',
      deleteable: true,
      addable: true,
      inputs: {
        numberA: {
          code: 'numberA',
          name: 'A',
          type: 'basic/number'
        },
        numberB: {
          code: 'numberB',
          name: 'B',
          type: 'basic/number'
        },
        equal: {
          code: 'equal',
          name: 'Can be equal',
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
    this.debug('execute', inputs)
    const a = inputs.numberA
    const b = inputs.numberB
    const eq = inputs.equal
    let res = false
    if (a > b)
      res = true
    if (eq && a === b)
      res = true
    this.setOutput('result', res)
  }
}

module.exports = NumberIsGreater
