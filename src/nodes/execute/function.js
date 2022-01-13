const AbstractNode = require('../abstract')

class CallFunction extends AbstractNode {

  static metadata() {
    return {
      name: 'CallFunction',
      code: 'graph/function',
      type: 'execute',
      deleteable: true,
      addable: false,
      inputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
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
    this.debug('execute', inputs)
    const paths = this._node.code.split('/')
    const outputs = await this._graph._vm.runLibraryFunction(this._node.data.library, this._node.data.fid, inputs)
    Object.keys(this._node.outputs).forEach(key => {
      const slot = this._node.outputs[key]
      if (slot.type === 'basic/execute') return
      this.setOutput(key, outputs[key])
    })
    return 'return'
  }
}

module.exports = CallFunction
