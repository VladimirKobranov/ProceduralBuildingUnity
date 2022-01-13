const AbstractNode = require('../abstract')

class IsUndefined extends AbstractNode {

  static metadata() {
    return {
      name: 'Is Undefined',
      code: 'is/undefined',
      type: 'modifier',
      deleteable: true,
      addable: true,
      inputs: {
        any: {
          code: 'any',
          name: 'Any',
          type: 'basic/template',
          template: 'any'
        }
      },
      outputs: {
        result: {
          code: 'result',
          name: 'Result',
          type: 'basic/boolean'
        }
      },
      templates: {
        any: {
          allow: ['*']
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    const ret = typeof inputs.any === 'undefined'
    this.setOutput('result', ret)
  }
}

module.exports = IsUndefined
