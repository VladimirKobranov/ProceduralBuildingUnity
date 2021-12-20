const AbstractNode = require('../abstract')

const waitMs = ms => new Promise(acc => setTimeout(acc,ms))

class Wait extends AbstractNode {

  static metadata() {
    return {
      name: 'Wait',
      code: 'datetime/wait',
      type: 'execute',
      deleteable: true,
      addable: true,
      outputs: {
        return: {
          code: 'return',
          name: 'Return',
          type: 'basic/execute'
        }
      },
      inputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        },
        ms: {
          code: 'ms',
          name: 'Ms',
          type: 'basic/number'
        }
      }
    }
  }

  async execute(inputs) {
    this.log('execute', inputs)
    await waitMs(inputs.ms || 0)
    return 'return'
  }
}

module.exports = Wait
