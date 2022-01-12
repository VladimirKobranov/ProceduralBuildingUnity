const dayjs = require('dayjs')
const AbstractModule = require('./abstract')

class ActorModule extends AbstractModule {

  static metadata() {
    return {
      code: 'core',
      name: 'Core',
      events: {
        start: {
          code: 'start',
          name: 'VM Start',
          outputs: {
            now: {
              code: 'now',
              name: 'Now',
              type: 'basic/datetime'
            }
          }
        }
      },
      classes: {
        EventEmitter: {
          code: 'EventEmitter',
          name: 'EventEmitter',
          methods: {
            emit: {
              code: 'emit',
              name: 'emit',
              inputs: {
                eventCode: {
                  code: 'eventCode',
                  name: 'Event Code',
                  type: 'basic/string'
                },
                eventData: {
                  code: 'eventData',
                  name: 'Event Data',
                  type: 'basic/template',
                  template: 'a'
                }
              },
              templates: {
                a: {
                  allow: ['*']
                }
              }
            },
            on: {
              code: 'on',
              name: 'on',
              inputs: {
                eventCode: {
                  code: 'eventCode',
                  name: 'Event Code',
                  type: 'basic/string'
                }
              },
              outputs: {
                subscribe: {
                  code: 'subscribe',
                  name: 'Subscribe',
                  type: 'basic/execute'
                },
                eventData: {
                  code: 'eventData',
                  name: 'Event Data',
                  type: 'basic/template',
                  template: 'a'
                }
              },
              templates: {
                a: {
                  allow: ['*']
                }
              }
            }
          }
        }
      }
    }
  }

  start () {
    const libs = this._vm.libraries()
    const functions = libs && libs.default && libs.default.functions ? libs.default.functions : {}
    const starts = Object.values(functions).filter(f => f.event && f.event.module === 'core' && f.event.code === 'start')
    const now = dayjs()
    starts.forEach(fn => {
      this._vm.runLibraryFunction('default', fn.code, {now})
    })
  }

}

module.exports = ActorModule
