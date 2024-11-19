<script>
    import { onMount } from "svelte";

    export let User = undefined
    let UserData = {
    username: "",
    password: ""
    }


    onMount(async () => {
      let response = await fetch("/api/auth")
      if (response.ok) {
        User = await response.json();
      }
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
        response.json().then(data => User = data)
      }
    }
</script>

<br>
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