const AbstractNode = require('../abstract')

class ClassThis extends AbstractNode {

  static metadata() {
    return {
      name: 'This',
      code: 'class/this',
      type: 'getter',
      deleteable: true,
      addable: false,
      outputs: {
        object: {
          code: 'object',
          name: 'this',
          type: 'bluep/class'
        }
      },
      inputs: {}
    }
  }

  async execute(inputs) {
    this.debug('execute', this._node.code)
    // console.log('execute THIS', this, )
    this.setOutput('object', this._context.self())
    // const val = this._context.getOutput(this._node.data.context, this._node.data.code)
    // this.setOutput(this._node.data.code, val)
  }
}

module.exports = ClassThis
