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
    const cls = this.vm().classCombined(this._node.data.class, this._node.data.library)
    const cn = cls.methods[this._node.data.code] || cls.deep.methods[this._node.data.code]
    const outs = await this.vm().runLibraryMethod(inputs.object, cn.library, cn.class, cn.code, fnInputs)
    Object.keys(this._node.outputs).forEach(fcode => {
      if (fcode === 'return') return
      this.setOutput(fcode, outs[fcode])
    })
    // console.log('method', inputs, outs)
    return 'return'
  }

}

module.exports = ClassMethod
