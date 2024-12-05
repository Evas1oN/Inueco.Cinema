<script>
    import { onMount } from "svelte";
    import { user } from "../stores/user";

    export let User = undefined
    let UserData = {
    username: "",
    password: ""
    }

    let oldway = false;

    onMount(async () => {
      let response = await fetch("/api/auth")
      if (response.ok) {
        user.set(await response.json());
        return;
      }
      throw(response.statusText)
    })

    /**
     * 
     * @param {Event} e
     */
    async function register(e) {
      e.preventDefault()

      await fetch("/api/users", { method: "POST", body: JSON.stringify(UserData), headers: { "Content-Type": "application/json" }})
      .then(res => res.json)
      .then(data => console.log(data))
    }

    async function login(e) {
      e.preventDefault();

      let response = await fetch("/api/auth", { method: "POST", body: JSON.stringify(UserData), headers: { "Content-Type": "application/json" }})
      if (response.ok) {
        response.json().then(data => user.set(data))
      }
    }
    
</script>

<input type="checkbox" bind:checked={oldway}/>

<br>

{#if oldway}
<div class="grid">
  <div>
      <h3>Регистрация</h3>
      <form on:submit={register}>
          <fieldset>
              <label>Имя пользователя
                <input bind:value={UserData.username}/>
              </label>
      
              <label>Пароль
                <input type="password" bind:value={UserData.password}/>
              </label>
          </fieldset>
      
          <button type="submit">Регистрация</button>
      </form>
  </div>


  <div>
      <h3>Вход</h3>
      <form on:submit={login}>
          <fieldset>
              <label>Имя пользователя
              <input bind:value={UserData.username}/>
              </label>
      
              <label>Пароль
              <input type="password" bind:value={UserData.password}/>
              </label>
          </fieldset>
      
          <button type="submit">Вход</button>
          </form>
  </div>
</div>
{:else }
<div>
  <h3>Вход</h3>
  <form>
      <fieldset>
          <label>Имя пользователя
          <input bind:value={UserData.username}/>
          </label>
  
          <label>Пароль
          <input type="password" bind:value={UserData.password}/>
          </label>
      </fieldset>
  
      <button on:click={login} type="submit">Вход</button>
      <button on:click={register} type="submit">Регистрация</button>
      </form>
</div>
{/if}
