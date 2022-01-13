const AbstractNode = require('../abstract')

// const { classCombined } = require('../../utils')

class New extends AbstractNode {

  static metadata() {
    return {
      name: 'New',
      code: 'class/new',
      type: 'execute',
      deleteable: true,
      addable: true,
      inputs: {
        call: {
          code: 'call',
          name: 'Call',
          type: 'basic/execute'
        },
        of: {
          code: 'of',
          name: 'Of',
          type: 'bluep/classselector',
        }
      },
      outputs: {
        return: {
          code: 'return',
          name: 'Return',
          type: 'basic/execute'
        },
        object: {
          code: 'object',
          name: 'Object',
          type: 'bluep/class',
        }
      }
    }
  }

  async execute(inputs) {
    this.debug('execute', inputs)
    if (typeof inputs.of !== 'string') return 'return'
    const codes = inputs.of.split('/')
    const fnInputs = {}
    Object.keys(inputs).forEach(incode => {
      if (incode === 'call' || incode === 'of') return
      fnInputs[incode] = inputs[incode]
    })
    const obj = {}
    const cls = this.vm().classCombined(this._node.data.class, this._node.data.library)
    // console.log('new::clsCombined', cls)
    obj._metainfo = cls
    Object.keys(cls.schema || {}).forEach(fcode => {
      obj[fcode] = cls.schema[fcode].value
    })
    Object.keys(cls.deep.schema || {}).forEach(fcode => {
      obj[fcode] = cls.deep.schema[fcode].value
    })
    const cn = cls.methods[this._node.data.fn] || cls.deep.methods[this._node.data.fn]
    if (cn) {
      await this.vm().runLibraryConstructor(obj, cn.library, cn.class, cn.code, fnInputs)
    }
    // console.log('new', obj, this._node.data)
    this.setOutput('object', obj)
    return 'return'
  }

}

module.exports = New
