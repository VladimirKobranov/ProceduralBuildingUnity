class Context {

  constructor(vm) {
    this._vm = vm
    this._outputs = {}
  }

  setOutput(node, slot, val) {
    if (!this._outputs[node])
      this._outputs[node] = {}
    this._outputs[node][slot] = val
  }

  getOutput(node, slot) {
    if (!this._outputs[node])
      return undefined
    return this._outputs[node][slot]
  }

  hasOutput(node) {
    return !!this._outputs[node]
  }

}

module.exports = Context
