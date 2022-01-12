const dayjs = require('dayjs')
const AbstractNode = require('../abstract')

class Now extends AbstractNode {

  static metadata() {
    return {
      name: 'Now',
      code: 'datetime/now',
      type: 'execute',
      deleteable: true,
      addable: true,
      outputs: {
        return: {
          code: 'return',
          name: 'Return',
          type: 'basic/execute'
        },
        datetime: {
          code: 'datetime',
          name: 'Datetime',
          type: 'basic/datetime'
        }
      },
      inputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    const ret = dayjs()
    this.setOutput('datetime', ret)
    return 'return'
  }
}

module.exports = Now
