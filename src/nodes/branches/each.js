const dayjs = require('dayjs')
const AbstractNode = require('../abstract')

class Each extends AbstractNode {

  static metadata() {
    return { name: 'Each',
      code: 'array/each',
      type: 'execute',
      deleteable: true,
      addable: true,
      inputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        },
        array: {
          code: 'array',
          name: 'Array',
          type: 'basic/template',
          template: 'A',
          isArray: true
        }
      },
      outputs: {
        loop: {
          code: 'loop',
          name: 'loop',
          type: 'basic/execute'
        },
        index: {
          code: 'index',
          name: 'index',
          type: 'basic/number'
        },
        element: {
          code: 'element',
          name: 'element',
          type: 'basic/template',
          template: 'A'
        },
        return: {
          code: 'return',
          name: 'Return',
          type: 'basic/execute'
        },
      },
      templates: {
        A: {
          allow: ['*'] // ,
          // disallow: [],
          // type: ''
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    if (!Array.isArray(inputs.array))
      return 'return'
    for (let i = 0; i < inputs.array.length; i++) {
      this.setOutput('index', i)
      this.setOutput('element', inputs.array[i])
      await this.callOutput('loop')
    }
    return 'return'
  }
}

module.exports = Each
