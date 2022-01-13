const AbstractNode = require('../abstract')

class If extends AbstractNode {

  static metadata() {
    return {
      name: 'If',
      code: 'branches/if',
      type: 'execute',
      deleteable: true,
      addable: true,
      inputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        },
        condition: {
          code: 'condition',
          name: 'Condition',
          type: 'basic/boolean'
        }
      },
      outputs: {
        ifTrue: {
          code: 'ifTrue',
          name: 'True',
          type: 'basic/execute'
        },
        ifFalse: {
          code: 'ifFalse',
          name: 'False',
          type: 'basic/execute'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('If::execute', inputs)
    if (inputs.condition)
      return 'ifTrue'
    return 'ifFalse'
  }
}

module.exports = If
