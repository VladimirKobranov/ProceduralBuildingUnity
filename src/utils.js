/**
 *  Cloning function used everywhere
 *  @param {T} obj - object to clone. by current implementation - should be "JSON compaible"
 *  @returns {T} cloned version of ``obj``
 */
const jclone = obj => JSON.parse(JSON.stringify(obj))

/**
 *  Class parents classes
 *  @param {string} classCode - class code
 *  @param {string} library - class library
 *  @param {object} libraries - libraries object
 *  @param {object} modules - modules metadata
 *  @returns {object} class parents. direct and deep extended.
 */
const classParents = (classCode, library, libraries, modules) => {
  const ret = { direct: [], back: [] }
  const cls = libraries[library].classes[classCode]
  if (!Object.keys(cls.extends || {}).length) return ret
  Object.values(cls.extends).forEach(ext => {
    const inf = { ...ext }
    if (ext.module && modules && modules[ext.module] && modules[ext.module].classes[ext.code]) {
      inf.src = modules[ext.module].classes[ext.code]
    }
    if (inf.library) {
      inf.src = libraries[inf.library].classes[inf.code]
      const parents = classParents(inf.code, inf.library, libraries, modules)
      ret.back = [...ret.back, ...parents.direct, ...parents.back]
    }
    ret.direct.push(inf)
  })
  ret.back = ret.back.filter((cls, i, self) => self.indexOf(cls) === i)
  ret.index = [...ret.direct, ...ret.back].map(c => {
    if (c.library) return `library/${c.library}/${c.code}`
    return `module/${c.module}/${c.code}`
  })
  return ret
}

/**
 *  Check if class is parent of other
 *  @param {string} classCode - code of "parent" class
 *  @param {string} classLibrary - library of "parent" class
 *  @param {string} childCode - code of "child" class
 *  @param {string} childLibrary - library of "child" class
 *  @param {object} libraries - libraries object
 *  @param {object} modules - modules metadata
 *  @returns {boolean} is parent or not
 */
const classIsParentOf = (classCode, classLibrary, childCode, childLibrary, libraries, modules) => {
  const parents = classParents(childCode, childLibrary, libraries, modules)
  return parents.index && parents.index.includes(`library/${classLibrary}/${classCode}`)
}

/**
 *  Class parents classes
 *  @param {string} classCode - class code
 *  @param {string} library - class library
 *  @param {object} libraries - libraries object
 *  @param {object} modules - modules metadata
 *  @returns {object|null} full class combined object
 */
const classCombined = (code, library, libraries, modules) => {
  if (!libraries[library].classes[code]) return null
  const cls = jclone(libraries[library].classes[code])
  const parents = classParents(code, library, libraries, modules)
  const list = [...parents.direct, ...parents.back]
  cls.deep = {
    schema: {},
    methods: {},
    overrides: {},
    parents: []
  }
  list.forEach(parent => {
    cls.deep.parents.push(parent.src.code)
    // properties
    Object.keys(parent.src.schema || {}).forEach(fld => {
      if (parent.src.schema[fld].access === 'private') return
      cls.deep.schema[fld] = jclone(parent.src.schema[fld])
      cls.deep.schema[fld].source = {
        library: parent.library,
        libraryName: libraries[parent.library].name,
        code: parent.src.code,
        name: parent.src.name
      }
    })
    Object.keys(parent.src.methods || {}).forEach(mcode => {
      if (parent.src.methods[mcode].access === 'private') return
      // parent method is overriden in class
      const over = Object.values(cls.methods || {}).find(m => m.overrides === mcode)
      if (over) {
        cls.deep.overrides[mcode] = jclone(parent.src.methods[mcode])
        cls.deep.overrides[mcode].source = {
          library: parent.library,
          libraryName: libraries[parent.library].name,
          code: parent.src.code,
          name: parent.src.name
        }
        return
      }
      // parent method is overriden in other parent class
      if (Object.values(cls.deep.methods || {}).find(m => m.overrides === mcode)) return
      // or already was checked and moved to overrides
      if (Object.values(cls.deep.overrides || {}).find(m => m.overrides === mcode)) return
      cls.deep.methods[mcode] = jclone(parent.src.methods[mcode])
      cls.deep.methods[mcode].source = {
        library: parent.library,
        libraryName: libraries[parent.library].name,
        code: parent.src.code,
        name: parent.src.name
      }
      // parent method overrides other deep method
      const po = parent.src.methods[mcode].overrides
      const md = cls.deep.methods[po]
      if (po && md) {
        const over = jclone(cls.deep.methods[po])
        delete cls.deep.methods[po]
        cls.deep.overrides[po] = over
      }
    })
  })
  return cls
}

module.exports = {
  jclone,
  classParents,
  classIsParentOf,
  classCombined
}
