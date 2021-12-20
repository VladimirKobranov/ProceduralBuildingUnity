const AbstractNode = require('../abstract')

class ConsoleLog extends AbstractNode {

  static metadata() {
    return {
      name: 'Console log',
      code: 'console/consolelog',
      type: 'execute',
      inputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        },
        message: {
          code: 'message',
          name: 'Message',
          type: 'basic/string'
        }
      },
      outputs: {
        return: {
          code: 'return',
          name: 'Return',
          type: 'basic/execute'
        }
      }
    }
  }

  async execute(inputs) {
    this.log('execute', inputs)
    this.vm().console().log(inputs.message)
    return 'return'
  }

}

module.exports = ConsoleLog
