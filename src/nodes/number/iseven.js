const AbstractNode = require('../abstract')

class NumberIsEven extends AbstractNode {

  static metadata() {
    return {
      name: 'Is Even',
      code: 'number/iseven',
      type: 'modifier',
      deleteable: true,
      addable: true,
      inputs: {
        number: {
          code: 'number',
          name: 'Number',
          type: 'basic/number'
        }
      },
      outputs: {
        iseven: {
          code: 'iseven',
          name: 'Is Even',
          type: 'basic/boolean'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    this.setOutput('iseven', inputs.number % 2 === 0)
  }
}

module.exports = NumberIsEven
