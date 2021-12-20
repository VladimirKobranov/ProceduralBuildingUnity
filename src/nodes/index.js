const ConsoleLog = require('./execute/consoleLog')
const Call = require('./execute/call')
const Return = require('./execute/return')
const Constructor = require('./execute/constructor')
const CallFunction = require('./execute/function')
const Now = require('./execute/now')
const Wait = require('./execute/wait')

const For = require('./branches/for')
const If = require('./branches/if')
const Each = require('./branches/each')

const VariableGet = require('./variable/get')
const VariableSet = require('./variable/set')

const EnumToString = require('./enum/enum2string')
const EnumEqual = require('./enum/eq')

const StructPack = require('./struct/pack')
const StructUnpack = require('./struct/unpack')
const StructToObject = require('./struct/toobject')
const StructFromObject = require('./struct/fromobject')

const NumberEq = require('./number/eq')
const NumberToString = require('./number/number2string')
const NumberIsEven = require('./number/iseven')
const NumberIsGreater = require('./number/gt')

const StringEq = require('./string/eq')
const StringAppend = require('./string/append')

const DatetimeCreate = require('./datetime/create')
const DatetimeUnwrap = require('./datetime/unwrap')
const DatetimeToString = require('./datetime/tostring')

const BooleanAnd = require('./boolean/and')
const BooleanOr = require('./boolean/or')
const BooleanNot = require('./boolean/not')
const BooleanEq = require('./boolean/eq')

const IsUndefined = require('./undefined/isundefined')

const Nodes = {
  Call,
  Return,
  Constructor,
  ConsoleLog,
  CallFunction,
  Now,
  Wait,

  VariableGet,
  VariableSet,

  StringAppend,

  EnumToString,
  EnumEqual,

  StructPack,
  StructUnpack,
  StructToObject,
  StructFromObject,

  For,
  If,
  Each,

  BooleanAnd,
  BooleanOr,
  BooleanNot,
  BooleanEq,

  NumberEq,
  NumberToString,
  NumberIsEven,
  NumberIsGreater,

  IsUndefined,

  DatetimeCreate,
  DatetimeUnwrap,
  DatetimeToString,
}

module.exports = Nodes
