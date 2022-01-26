const AbstractNode = require('../abstract')

class Constructor extends AbstractNode {

  static metadata() {
    return {
      name: 'Constructor',
      code: 'class/Constructor',
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
    this.debug('execute', { inputs, data: this._node.data })
    const cls = this.vm().classCombined(this._node.data.class)
    if (!cls) {
      this.error('parent class not found', this._node.data.class)
      return 'return'
    }
    const cn = cls.methods[this._node.data.code] || null
    if (!cn) {
      this.error('parent constructor not found', this._node.data.code)
      return 'return'
    }
    const obj = this.context().self()
    if (!obj) {
      this.error('super constructor called without self object!', this._node.data)
      return 'return'
    }
    const fnInputs = {}
    Object.keys(inputs).forEach(incode => {
      if (incode === 'call') return
      fnInputs[incode] = inputs[incode]
    })
    // no need to set/modify "self" fileds - this is done in "new" node
    await this.vm().runLibraryConstructor(obj, cn.library, cn.class, cn.code, fnInputs)
    return 'return'
  }
}

module.exports = Constructor
