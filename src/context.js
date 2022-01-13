class Context {

  constructor(vm, self) {
    this._vm = vm
    this._outputs = {}
    this._self = self
  }

  self() { return this._self }

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
