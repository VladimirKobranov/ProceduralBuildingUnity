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
          type: 'basic/string',
          multiple: 'message'
        }
      },
      outputs: {
        return: {
          code: 'return',
          name: 'Return',
          type: 'basic/execute'
        }
      },
      multiples: {
        'message': { value: 1 }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    let msg = ''
    Object.keys(inputs || {}).forEach(key => {
      if (key.startsWith('message')) msg += inputs[key]
    })
    this.vm().console().log(msg)
    return 'return'
  }

}

module.exports = ConsoleLog
