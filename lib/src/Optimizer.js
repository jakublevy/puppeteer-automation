import './JsExtensions/ArrayExtensions.js'

class Optimizer {

    optimize(acts) {
        for(let i = acts.length - 1; i >= 0; --i) {
            if(i - 1 >= 0) {
                if (acts[i].type === 'pageUrlChanged' && acts[i-1].type === 'submit')
                    this._addImplicitNavigationHint(acts, i)

                else if (acts[i].type === 'pageUrlChanged' && acts[i - 1].type === 'click')
                    this._addImplicitNavigationHint(acts, i)

                else if (acts[i].type === 'submit' && acts[i - 1].type === 'click') {
                    /* selectors won't match, click will match button selector, submit will match form selector */
                    if(!acts[i - 1].isTrusted) {
                        acts.removeRange(i - 1, i)
                        continue
                    }

                    else {
                        acts.removeAt(i)
                        continue
                    }
                }
                else if(acts[i].type === 'click' && acts[i-1].type === 'change' && this._theSameElement(acts[i-1], acts[i])) {
                    acts.removeAt(i)
                    continue
                }

                else if(acts[i].type === 'change') {
                    if(acts[i-1].type === 'click' && this._theSameElement(acts[i-1], acts[i])) {
                        acts.removeAt(i-1)
                        continue
                    }

                    else if(acts[i-1].type === 'paste') {
                        acts.removeAt(i)
                        continue
                    }
                }

                else if( (acts[i - 1].type === 'pageOpened')
                    && acts[i].type === 'pageSwitched' ) {
                    this._addImplicitPageSwitchHint(acts, i)
                }
                else if(acts[i-1].type === 'pageClosed' && acts[i].type === 'pageSwitched'
                     && acts[i-1].idx === acts[i].oldIdx) {
                    this._addImplicitPageSwitchHint(acts, i)
                }
            }

            if(acts[i].type === 'input') {
                let j = i
                if(i + 1 >= acts.length || acts[i+1].type !== 'change') {
                    //make acts[i] change
                    acts[i].type = 'change'
                    --j
                }

                //delete input chain
                while (j >= 0 && acts[j].type === 'input') {
                    acts.removeAt(j)
                    --j
                }
                i = j + 2

            }

            else if(acts[i].type === 'dblclick') {
                let clicksRemoved = 0
                let j = i - 1
                while(clicksRemoved < 2 && j >= 0) {
                    if(acts[j].type === 'click' && acts[j].selector === acts[i].selector) {
                        acts.removeAt(j)
                        --i
                        ++clicksRemoved
                    }
                    else if(acts[j].type === 'pageUrlChanged' && acts[j].implicitNav && clicksRemoved > 0){
                        acts.insertAt(i+1, acts[j])

                        acts.removeAt(j)
                    }
                    --j
                }
                i = j + 1
            }
            else if(acts[i].type === 'scroll') {
                let j = i - 1
                while(j >= 0 && acts[j].type === 'scroll') {
                    acts.removeAt(j)
                    --j
                }
                i = j + 1
            }
        }


        // for (let i = 0; i < acts.length; ++i) {
        //     if(i + 1 < acts.length) {
        //         if (acts[i].type === 'submit' && acts[i + 1].type === 'pageUrlChanged') {
        //             this._addImplicitNavigationHint(acts, i+1)
        //             //acts.removeAt(i + 1)
        //             //--i
        //         }
        //         else if (acts[i].type === 'click' && acts[i + 1].type === 'submit')
        //             /* && acts[i].selector === acts[i+1].selector)
        //             TODO: selectors won't match, click will match button selector, submit will match form selector */ {
        //                 if(!acts[i].isTrusted) {
        //                     acts.removeRange(i, i+1)
        //                     i -= 2
        //                 }
        //                 else {
        //                     acts.removeAt(i)
        //                     --i
        //                 }
        //         }
        //
        //         else if (acts[i].type === 'click' && acts[i + 1].type === 'pageUrlChanged') {
        //             this._addImplicitNavigationHint(acts, i+1)
        //             //acts.removeAt(i + 1)
        //             //--i
        //         }
        //
        //         else if( (acts[i].type === 'pageOpened' || acts[i].type === 'pageClosed')
        //                && acts[i+1].type === 'pageSwitched' ) {
        //             this._addImplicitPageSwitchHint(acts, i + 1)
        //         }
        //
        //     }
        //     if(acts[i].type === 'dblclick') {
        //         let clicksRemoved = 0
        //         let j = i - 1
        //         while(clicksRemoved < 2 && j >= 0) {
        //             if(acts[j].type === 'click' && acts[j].selector === acts[i].selector) {
        //                 acts.removeAt(j)
        //                 --i
        //                 ++clicksRemoved
        //             }
        //             else if(acts[j].type === 'pageUrlChanged' && acts[j].implicitNav && clicksRemoved > 0){
        //                 acts.insertAt(i+1, acts[j])
        //                 acts.removeAt(j)
        //                 --i
        //             }
        //             --j
        //         }
        //     }
        //
        //
        //     else if(this._keyPressChangeOpt(acts, i)) {
        //        acts.dropWhile(act => act.type === 'keypress', i)
        //         --i
        //     }
        //     // else if(this._inputChangeOpt(acts, i)) {
        //     //     //TODO:
        //     // }
        //     // else if(this._selectChangeOpt(acts, i)) {
        //     //     //TODO:
        //     // }
        // }
        return acts
    }
    // _keyPressChangeOpt(acts, i) {
    //     const pressedKeys = this._pressedSeq(acts, i)
    //     const nextActionIdx = i + pressedKeys.length
    //     return nextActionIdx < acts.length && acts[nextActionIdx].type === 'change' && acts[nextActionIdx].value === pressedKeys
    // }
    //
    // _pressedSeq(acts, i) {
    //     let keys = ''
    //     while(i < acts.length && acts[i].type === 'keypress') {
    //         keys += String.fromCharCode(acts[i].keyCode)
    //         ++i
    //     }
    //     return keys
    // }

    _theSameElement(act1, act2) {
        return JSON.stringify(act1.locators) === JSON.stringify(act2.locators) || act1.selector === act2.selector
    }

    _addImplicitNavigationHint(acts, idx) { acts[idx].implicitNav = true }
    _addImplicitPageSwitchHint(acts, idx) { acts[idx].implicitSwitch = true }
}

export default Optimizer