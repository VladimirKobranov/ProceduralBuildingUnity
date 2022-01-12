const AbstractNode = require('../abstract')

class For extends AbstractNode {

  static metadata() {
    return {
      name: 'For',
      code: 'loops/for',
      type: 'execute',
      deleteable: true,
      addable: true,
      inputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        },
        from: {
          code: 'from',
          name: 'From',
          type: 'basic/number'
        },
        to: {
          code: 'to',
          name: 'To',
          type: 'basic/number'
        },
        step: {
          code: 'step',
          name: 'Step',
          type: 'basic/number'
        },
        incTo: {
          code: 'incTo',
          name: 'Include to',
          type: 'basic/boolean'
        }
      },
      outputs: {
        body: {
          code: 'body',
          name: 'Body',
          type: 'basic/execute'
        },
        index: {
          code: 'index',
          name: 'Index',
          type: 'basic/number'
        },
        done: {
          code: 'done',
          name: 'Done',
          type: 'basic/execute'
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('For::execute', inputs)
    if (typeof inputs.from !== 'number'
        || typeof inputs.to !== 'number'
        || typeof inputs.step !== 'number')
      return 'done'
    if (inputs.from < inputs.to ) {
      if (inputs.incTo) {
        for (let i = inputs.from; i <= inputs.to; i += inputs.step) {
          this.setOutput('index', i)
          await this.callOutput('body')
        }
      } else {
        for (let i = inputs.from; i < inputs.to; i += inputs.step) {
          this.setOutput('index', i)
          await this.callOutput('body')
        }
      }
    } else {
      if (inputs.incTo) {
        for (let i = inputs.from; i >= inputs.to; i -= inputs.step) {
          this.setOutput('index', i)
          await this.callOutput('body')
        }
      } else {
        for (let i = inputs.from; i > inputs.to; i -= inputs.step) {
          this.setOutput('index', i)
          await this.callOutput('body')
        }
      }
    }
    return 'done'
  }
}

module.exports = For
