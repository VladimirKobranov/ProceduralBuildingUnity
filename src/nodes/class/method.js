const AbstractNode = require('../abstract')

class ClassMethod extends AbstractNode {

  static metadata() {
    return {
      name: 'ClassMethod',
      code: 'class/method',
      type: 'execute',
      deleteable: true,
      addable: false,
      inputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        },
        object: {
          code: 'object',
          name: 'Object',
          type: 'bluep/class'
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
    const fnInputs = {}
    Object.keys(inputs).forEach(incode => {
      if (incode === 'call' || incode === 'of') return
      fnInputs[incode] = inputs[incode]
    })
    const self = inputs.object
    if (typeof self.constructor.metadata === 'function' && typeof self.method === 'function') {
      // has static metadata method.
      // can't be object of class fully described by blueprints
      // and should be an actor
      // cuz also has 'method' method.
      // call it directly
      const outs = await self.method(this._node.data.code, fnInputs)
      Object.keys(this._node.outputs).forEach(fcode => {
        if (fcode === 'return') return
        this.setOutput(fcode, outs[fcode])
      })
      return 'return'
    }
    
    const cls = !!this._node.data.strict
      ? this.vm().classCombined(this._node.data.class)
      : this.vm().classCombined((self._metadata || {}).code)
    /**/
    const over = Object.values(cls.methods || {}).find(m => m.overrides === this._node.data.code)
    const fnCode = !!this._node.data.strict
      ? this._node.data.code
      : !!over
        ? over.code
        : this._node.data.code
    /**/
    const cn = cls.methods[fnCode] || cls.deep.methods[fnCode]
    if (!cn) {
      this.error('method not found!', this._node.data.code)
      return 'return'
    }
    const outs = await this.vm().runLibraryMethod(inputs.object, cn.library, cn.class, cn.code, fnInputs)
    Object.keys(this._node.outputs).forEach(fcode => {
      if (fcode === 'return') return
      this.setOutput(fcode, outs[fcode])
    })
    return 'return'
  }

}

module.exports = ClassMethod
