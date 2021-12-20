const AbstractNode = require('../abstract')

class StringAppend extends AbstractNode {

  static metadata() {
    return {
      name: 'A+B',
      code: 'string/append',
      type: 'modifier',
      inputs: {
        source: {
          code: 'source',
          name: 'A',
          type: 'basic/string'
        },
        append: {
          code: 'append',
          name: 'B',
          type: 'basic/string'
        }
      },
      outputs: {
        result: {
          code: 'result',
          name: 'Result',
          type: 'basic/string'
        }
      }
    }
  }

  async execute(inputs) {
    this.log('execute', inputs)
    this.setOutput('result', `${inputs.source}${inputs.append}`)
  }
}

module.exports = StringAppend
