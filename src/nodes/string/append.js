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
          type: 'basic/string',
          multiple: 'A'
        }
      },
      outputs: {
        result: {
          code: 'result',
          name: 'Result',
          type: 'basic/string'
        }
      },
      multiples: {
        A: {
          min: 1,
          value: 1
          // max: 10
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    let ret = inputs.source + inputs.append
    Object.keys(inputs).forEach(inp => {
      if (inp.startsWith('append_multiple_')) ret += inputs[inp]
    })
    this.setOutput('result', ret)
  }
}

module.exports = StringAppend
